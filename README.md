TitleCleaner
============

Cleans and renames the titles of downloaded media files (tv/movies).
Also incorporates functionality to retrieve missing TV Show titles from the TVDB as well as relocate files to appropriate storage locations (eg rename and move from a downloads folder to some media directory).

If there is insufficient information the program will make a best effort approach when trying to find names.

CLI:<br />
---
Download latest release [here](https://github.com/mrkno/TitleCleaner/releases/download/v2.0-beta.1/TitleCleaner.zip)
<br /><code>tc-cli.exe [OPTION]...</code>

	If no options are specified defaults will be used.
	-m, --mode
		The {MODE} to run this application in (normal/test).
		
	-t, --type
		The {TYPE} of media files being input (tv/movie/auto).
		
	-i, --tvdb
		Retreives new missing tv episode names from the TVDB.
		
	-c, --confirm
		Asks for confirmation on rename/move/test.
		
	-o, --out
		Move media files to {DIRECTORY}. In test mode this outputs the outcomes
		of the test cases to the specified file.
		
	-s, --format
		{FORMAT} to use for output file naming.
		
	-v, --tformat
		{FORMAT} to use for output TV file naming. This option overrides the -s option.
		
	-e, --mformat
		{FORMAT} to use for output movie file naming. This option overrides the -s option.
		
	-h, --help
		Display this help.
		
	-d, --directory
		The {DIRECTORY} to search for files. When used the directory provided will be
		searched instead of the current directory for media files.
		This option is mutually exclusive with -f.
		
	-f, --file
		Cleans an individual {FILE} instead of searching. The behaviour of the application
		is otherwise unchanged. This option is mutually exclusive with -d. In test mode
		this specifies a test .csv file to run.

This is an improved version of the original mess located here:
https://code.google.com/p/title-cleaner

Any contributions/pull requests welcome.

License changed to the MIT Licence.
