# xDav

xDav is .net server mudole for webdav standard . its allow you to handle your webdav requests easily

you can get the a liberary and webconfig settings from nuget:

```sh
PM> Install-Package xDav
```

after installin that package your web.config has changed
```sh
  <configSections>
    <section name="XDavConfig" type="XDav.Config.XDavConfig, XDav" allowLocation="true" allowDefinition="Everywhere"/>
  </configSections>
```
this is the config section, and doesn't need any changes

```sh
  <system.webServer>
    <modules>
      <add name="XDav" type="XDav.XDavModule, XDav"/>
    </modules>
  </system.webServer>
```
it contains Adding xDav HttlModule and no need any changes

```sh
  <XDavConfig Name="xdav">
    <FileLocation URL="xdav" PathType="Local"></FileLocation>
  </XDavConfig>
```
the "Name" meens is when your request contans "/xdav/" [ value of "Name"], this request handled by xDav

