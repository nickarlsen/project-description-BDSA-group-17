Drop Table if exists GitRepos;
Drop Table if exists Commits;


Create Table GitRepos (
	owner VARCHAR(50),
	name VARCHAR(50),
	Primary Key(owner, name)
);

Create Table Commits (
	id INTEGER Primary Key,
	CONSTRAINT repoKey REFERENCES GitRepos(owner, name),
	author VARCHAR(50),
	commitDate DATE
);

Drop Table if exists GitRepos
Drop Table if exists Commits


Create Table Commits (
	id INTEGER Primary Key,
	repoOwner VARCHAR(50),
	repoName VARCHAR(50),
	author VARCHAR(50),
	commitDate DATE,
	Foreign Key (repoOwner, repoName) References GitRepos(owner, name)
)


Create Table GitRepos (
	owner VARCHAR(50),
	name VARCHAR(50),
	Primary Key(owner, name)
)