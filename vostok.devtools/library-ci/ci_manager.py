import requests
import os
import tempfile
import subprocess
import sys
import shutil

SECRET = "<secret>"

GITHUB_PERSONAL_ACCESS_TOKEN = SECRET


def normalize_repo_name(repo):
    return repo.replace("vostok.", "")


# For Windows
def override_ci_in_repo(repo, url, workflow_content):
    dst = 'C:\\t'
    repo_path = os.path.join(dst, normalize_repo_name(repo))

    if os.path.exists(repo_path):
        subprocess.call(f'cmd /D /C "cd "{repo_path}" && "C:\Program Files\Git\cmd\git.exe" pull"', shell=True)
    else:
        subprocess.call(f'cmd /D /C "cd "{dst}" && "C:\Program Files\Git\cmd\git.exe" clone {url}"', shell=True)

    workflow_path = os.path.join(repo_path, ".github", "workflows", "ci.yml")
    if not os.path.exists(workflow_path):
        print(f"Skipping (no workflow)")
        return
    with open(os.path.join(workflow_path), 'w') as f:
        f.write(workflow_content)
    subprocess.call(f'cmd /D /C "cd "{repo_path}" ' +
                    f'&& "C:\Program Files\Git\cmd\git.exe" add . && ' +
                    f'"C:\Program Files\Git\cmd\git.exe" commit -m "Update ci" && ' +
                    f'"C:\Program Files\Git\cmd\git.exe" push"', shell=True)


def override_ci_in_all_repos(github, repos, github_repos):
    workflow_content = github.get_actual_workflow()
    for i, repo in enumerate(repos):
        print(f"Processing {repo}... ({i + 1} out of {len(repos)})")
        override_ci_in_repo(repo, github_repos[repo], workflow_content)


# See https://docs.github.com/en/rest/reference/repos
class GithubClient:
    def __init__(self, token=None):
        self.personal_access_token = token

    def check_secrets(self):
        if not self.personal_access_token or self.personal_access_token == SECRET:
            print("Github personal access token is not specified!")
            sys.exit(1)

    def parse_repositories(self, data):
        return [(repo['full_name'].replace('/', '.'), repo['ssh_url']) for repo in data]

    def get_vostok_repositories(self):
        url = 'https://api.github.com/orgs/vostok/repos?per_page=100000'

        return {repo: url for repo, url in self.parse_repositories(requests.get(url=url).json())}

    def build_headers(self):
        self.check_secrets()

        return {"Authorization": "token {}".format(self.personal_access_token)}

    def get_webhooks_from(self, repo):
        headers = self.build_headers()

        url = f"https://api.github.com/repos/vostok/{repo}/hooks"

        return [(hook["name"], hook["id"]) for hook in requests.get(url=url, headers=headers).json()]

    def toggle_webhook(self, repo, id, enabled):
        headers = self.build_headers()

        url = f"https://api.github.com/repos/vostok/{repo}/hooks/{id}"

        active_state = "true" if enabled else "false"
        data = f'{{"active": {active_state}}}'

        return requests.patch(url=url, headers=headers, data=data).json()

    def get_actual_workflow(self):
        url = "https://raw.githubusercontent.com/vostok/devtools/master/library-ci/github_ci.yml"

        return requests.get(url=url).text


github = GithubClient(GITHUB_PERSONAL_ACCESS_TOKEN)

github_repos = github.get_vostok_repositories()


def get_intersection_repos():
    repos = []

    for repo in sorted(github_repos.keys()):
        repos.append(repo)

    return repos


repos = get_intersection_repos()

print("\n")
print(f"Ready to add/override CI in the following {len(repos)} repos:", end='\n\t')
print("\n\t".join(repos))
print("\n")
print("Please check carefully that all desired repositories are present and uncomment desired operation.")

override_ci_in_all_repos(github, repos, github_repos)
