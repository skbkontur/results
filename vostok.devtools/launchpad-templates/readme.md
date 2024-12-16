## Launchpad templates

A template is a NuGet package with following properties:

* All of its contents are located in `contentFiles` folder of the package.
* It can contain nested files and directories.
* It can contain variable placeholders in file/directory names and content.
* Variable placeholders should be enсlosed in double curly braces: `{{VariableName}}`
* It should contain a special file called `launchpad.json` in the root directory of template.
  * This file is called a template spec and should be in JSON format.
  * It should contain definitions of all the variables used in template ([example](library-ordinary/template/launchpad.json)).

See [Vostok library template](library-ordinary) for a working example.
