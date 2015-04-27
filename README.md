# UpdateLib

UpdateLib is an automatically update library for WPF-based applications inspired from AppCast technology.

## 1. Create release notes html file


```html
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Release notes</title>
</head>
<body>
    <h2>Release notes:</h2>

    <h3>2.13.2</h3>
    <p>Fixed something again</p>

    <h3>2.13.1</h3>
    <p>Fixed something</p>
</body>
</html>
```

## 2. Create appcast xml file

```xml
<?xml version="1.0" encoding="utf-8"?>
<item>
    <title>New version 2.13.2 available!</title>
    <version>2.13.2</version>
    <url>https://github-windows.s3.amazonaws.com/GitHubSetup.exe</url>
    <changelog>https://windows.github.com/release-notes.html</changelog>
</item>
```

## 3. Add Updatelib to project dependencies


## 4. Provide your appcast url into AutoUpdater in C# code


```csharp
AutoUpdater.AppCastUrl = "https://gist.githubusercontent.com/Snegovikufa/dbba6461db04bc7eb2c0/raw/605116ba229afd0600ad2e832c2bcecf4ef10d33/appcast.xml";
AutoUpdater.Start();
```