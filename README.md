XDav
====

xDav is a .Net Module For WebDAV Standard base on Http Protocol ,you can check the source code and the example to see how does it work 


Web Config:
---
<pre>
Add this section at the first of Configuration Node 
 <configSections>
    <section name="XDavConfig" type="XDav.Config.XDavConfig, XDav" allowLocation="true" allowDefinition="Everywhere"/>
  </configSections>
  
  Add this Section To your Configuration File too
    <XDavConfig Name="xdav">
    <FileLocation URL="xdav" PathType="Local"></FileLocation>
  </XDavConfig>
    </pre>
    
Name="xdav"

This is a key that couses to xDav module find the WebDAV requests, it means when your request contains '/xdav/filename.docx', that requests handle with xDav module

------------------------
When you set PathType as "Local" it means you have a foldet in your root web Folder with "URL" name,
FileLocation URL="xdav" PathType="Local"

And when you set PathType as "Server" you have enter full Server path in "URL" like:
FileLocation URL="c:\webdav" PathType="Local" 


And you should Add module settings to your Wen.Config
<system.webServer>
    <modules>
      <add name="XDav" type="XDav.XDavModule, XDav"/>
    </modules>
  </system.webServer>
  
  
