# GitInsight Docuementation

## Requirements

[label](https://markdownlivepreview.com/)

| Requirement  | Functional/Non-functional  |
|---|---|
|The application should receive the path to a Git repository that resides in a local directory   | Functional  |
| The application should collect all commits with respective author names and author dates  | Functional   |
| The application should be able to run in two different modes; commit frequency mode and commit auhtor mode | Functional  |
| Commit frequency mode should produce textual output on stdout that lists the number of commits per day  | Functional  |
|Author frequency mode should produce textual output on stdout that lists the number of commits per day per author   | Functional  |
|Unit tests should be run on every push to the main branch or on any pull-request that is merged into the main branch | Non-functional  |
| The application should be build on every push to the main branch or on any pull-request that is merged into the main branch, to ensure that it builds correctly  | Non-functional  |
| The collected data should be stored inside of a database  | Non-functional  |
| The application should recognize repositories which data it has already collected, and when collecting from these only the new data should be added on top of the prior data  | Non-functional |
| In the case of collecting data from a repository in which the version correspond to the most current state of the repository, the analysis step should be skipped entirely and the output should be generated from the readily available data directly | Non-functional  |
|   |   |

## Notes in General

No notes here.. yet :)
