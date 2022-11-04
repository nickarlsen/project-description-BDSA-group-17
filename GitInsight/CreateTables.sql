
Create Table GitRepos (
	url INTEGER Primary Key,
	name VARCHAR(50)
	
);

Create Table Commits (
	id INTEGER Primary Key,
	repoId INTEGER REFERENCES GitRepos(id),
	author VARCHAR(50),
	commitDate DATE
	
);