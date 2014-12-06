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
the "Name" means is when your request contans "/xdav/" [ value of "Name"], this request handled by xDav

FileLocationURL :
when you use <b> Local </b> as "PathType" it means you have a folder with "URL" property name in your web root, and when you user <b> Server </b> as "PathType" it means you entered a physical address like "c:\webdav\" in "URL" Property, and this folder includes your files.

Events
===
you can Add a class and call it on your <b><i>Global.ascx</i></b> like :
```sh
XdavConfig.Register();
```
and this file contains this section :
```sh
public class XdavConfig
    {
        public static void Register()
        {
            XDavSettings.Events(e => {
                e.OnProcessing(evt =>
                {
                    // Do this if you want cancel xdav process
                    //evt.CancelProcess();
                })
                .OnProcessed(evt =>
                {

                })
                .OnException(ex => { 

                });
            });
        }
    }
```

you can find a persian Article 
<a href="http://www.dotnettips.info/post/1923/%D8%A2%D8%B4%D9%86%D8%A7%DB%8C%DB%8C-%D8%A8%D8%A7-webdav-%D9%88-%D9%86%D8%AD%D9%88%D9%87-%D8%A7%D8%B3%D8%AA%D9%81%D8%A7%D8%AF%D9%87-%D8%A7%D8%B2-%D8%A2%D9%86"> Here </a>

-----
Reza Bazargan


