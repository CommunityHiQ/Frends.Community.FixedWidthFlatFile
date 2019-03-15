# Frends.Community.FixedWidthFlatFile

Task for parsing fixed width flat files

- [Installing](#installing)
- [Task](#tasks)
	- [Parse](#parse)
- [Building](#building)
- [Contributing](#contributing)
- [Change Log](#change-log)

# Installing

You can install the task via FRENDS UI Task View or you can find the nuget package from the following nuget feed
'Insert nuget feed here'

# Task

## Parse

Parses fixed width flat file to object with possibility to convert to JSON and XML.

### Properties

#### Input

| Property    | Type       | Description     | Example |
| ------------| -----------| --------------- | ------- |
| Flat file content | string | Fixed width flat file data to parse. | |
| Header row | enum<None, FixedWidth, Delimited> | Flat files header row type. | FixedWidth |
| Header delimiter | char | If header row is of type Delimited, set delimiter char here. | ; |
| Column specification | array | Array of column specifications. see Column specification item(#column-specification-item) for details. | |

##### Column specification Item

| Property    | Type       | Description     | Example |
| ------------| -----------| --------------- | ------- |
| Name | string | Column name. If left empty and header row is present, header value is used. If left empty and no header row is present, uses default name 'Field_1'. | |
| Type | enum <String, Int, Long, Decimal, Double, Boolean, DateTime, Char> | The type of the column. | 'String' |
| Date time format | string | If column type is set to date time, the actual format is set here for parsing the value. | 'yyyyMMdd' |
| Length | int | The length of the column in input fixed width data. | 8 |

#### Options

| Property    | Type       | Description     | Example |
| ------------| -----------| --------------- | ------- |
| Skip rows | boolean | Toggle skipping of data rows. | false |
| Skip rows from top | int | If skip rows is true, task skips the given amount of data rows from top. | 2 |
| Skip rows from bottom | int | If skip rows is true, task skips the given amount of rows from end. | 3 |
| Culture info | string | Specify the culture info to be used when parsing result to Json | 'fi-FI' |


#### Result

Example fixed width input data:

```
Timestamp;User
05-10-2017Veijo Frends    
```

would return object with property *Data* that has one row with keys "Timestamp" and "User".

You can access *Timesamp* value, for example, as follows:

```
#result.Data[0]["Timestamp"]
```


Calling *#result.ToJson()* would return:
```
[
	{
		"Timestamp": "2017-10-05T00:00:00",
		"User": "Veijo Frends"
	}
]
```

Calling *#result.ToXml()* would return:
```
<?xml version="1.0" encoding="utf-8"?>
<Root>
  <Rows>
    <Row>
      <Timestamp>2017-10-05T00:00:00</Timestamp>
      <User>Veijo Frends</User>
    </Row>
  </Rows>
</Root>
```


# Building

Clone a copy of the repo

`git clone https://github.com/CommunityHiQ/Frends.Community.FixedWidthFlatFile.git`

Restore dependencies

`nuget restore Frends.Community.FixedWidthFlatFile`

Rebuild the project

Run Tests with nunit3. Tests can be found under

`Frends.Community.FixedWidthFlatFile.Tests\bin\Release\Frends.Community.FixedWidthFlatFile.Tests.dll`

Create a nuget package

`nuget pack nuspec/Frends.Community.FixedWidthFlatFile.nuspec`

# Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

# Change Log

| Version | Changes |
| ----- | ----- |
| 1.0.0 | Initial version of Fixed Width Flat File Task |
| 1.1.0 | Fixed bug where empty rows resulted in exception |
| 1.2.0 | Fixed bug where ToJson() and ToXml() methods failed with empty/null values |
