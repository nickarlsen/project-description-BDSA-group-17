Drop Table if exists GitRepos;
Drop Table if exists Commits;


Create Table GitRepos (
	owner VARCHAR(50),
	name VARCHAR(50),
	Primary Key(owner, name)
);

Create Table Commits (
	id INTEGER Primary Key,
	CONSTRAINT repoKey INTEGER REFERENCES GitRepos(owner, name),
	author VARCHAR(50),
	commitDate DATE
);