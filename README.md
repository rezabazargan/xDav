xDav
====

A .Net Module  For WebDav 




Web Config:

Add this section at the first of Configuration Node 
 <configSections>
    <section name="XDavConfig" type="XDav.Config.XDavConfig, XDav" allowLocation="true" allowDefinition="Everywhere"/>
  </configSections>
  
  Add this Section To your Configuration File too
    <XDavConfig Name="xdav">
    <FileLocation URL="xdav" PathType="Local"></FileLocation>
  </XDavConfig>
  
  
Name="xdav"

this is a key that couses to xDav module find the webdave requestes, it meand when your request contains '/xdav/filename.docx', that requests handle with xDav module

------------------------
when you set PathType as "Local" it means you have a foldet in your root web Folder with "URL" name,
<FileLocation URL="xdav" PathType="Local"></FileLocation>

and when you set PathType as "Server" you have enter full Server path in "URL" like :
<FileLocation URL="c:\webdav" PathType="Local"></FileLocation>


And you should Add module settings to your Wen.Config
<system.webServer>
    <modules>
      <add name="XDav" type="XDav.XDavModule, XDav"/>
    </modules>
  </system.webServer>
  
  
