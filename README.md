Code Generator

This project is here to provide Database-first code generation. 

I've used the Razor templating engine as the backbone of this project. The current version of this project is a bit old and is a little slow. It is not without warts because the Razor engine was created to generate dynamic HTML, not c# and sql. However, I've gotten a lot of use out of this tool and the pros hugely out-weigh the cons.

I'm not sure the CLI part even works. It was an idea and it never got finished. I generally use a config file (or several) to manage the settings for the current generation that I'm doing. I usually run the .\generateCode.ps1 from a VS Code terminal while looking at the config file.

I'd like to upgrade this tool to .net core and make it faster. The number of files needed in the install folder is simply ridiculous, but that has more to do with the version of Razor that I'm using.


Powershell Requirements

	Install-Module -Name SqlServer

	
Some notes on the config file:

	Connection String
		Must be in the following format:
			"Server=;Initial Catalog=;Integrated Security=SSPI;"
			
			You don't have to use SSPI, but I'm assuming that is how you will connect

	Post Processing Scripts
		CopyFiles.ps1
			- copies the generated files into your project. If you want to do this manually, remove this script.
		
		UpdateSQLProject.ps1
			- executes the generated sql files on the sql server. This way, you get your stored procs and everything without much effort.


		
