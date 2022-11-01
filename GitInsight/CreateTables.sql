Create Table GitRepos (
	id INTEGER Primary Key
	name VARCHAR(50)
	
)

Create Table Commits (
	id INTEGER Primary Key
	repoId INTEGER Foreign Key
	author VARCHAR(50)
	commitDate DATE
	
)