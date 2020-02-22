# GeoHash
A command-line tool to generate geohash (the xkcd kind) coordinates.

## Background
Geohashing is a bit like geocaching, where the goal is to find a certain place.
The difference is that with geohashing, the point you try to reach has been randomly generated.
It was invented by Randall Munroe in the [webcomic xkcd](https://xkcd.com/426/) and soon a whole community had been created around it.
Read more here: https://geohashing.site/geohashing/Main_Page

## This repository
This is an implementation of The Algorithm written in C#, using dotnet core 3.1.
It is a command-line application that for each invocation can return the coordinates for one geohash point.

It is written to be simple to reuse (just ~~steal~~copy the files geohash.cs and gdate.cs) and contains a test project that, among other things, tests that the tool is W30-compliant by generating and comparing all the coordinates in the table on https://geohashing.site/geohashing/30W_Time_Zone_Rule.

## Usage
The command-line tool works like this:

```
        geohash lat long <yyyy-mm-dd>
  
where   lat, long is integer e.g 59 12
        long is positive east of Greenwich
        if date is omitted, use current

or
        geohash -g <yyyy-mm-dd> 
        
        for globalhash
        if date is omitted, use current
```
