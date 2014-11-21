XDav
====

xDav is a .Net Module For WebDAV Standard base on Http Protocol ,you can check the source code and the example to see how does it work 


Web Config:
---
Add this section at the first of Configuration Node <pre>
<code>

 < configSections>
< section name="XDavConfig" type="XDav.Config.XDavConfig, XDav" allowLocation="true" allowDefinition="Everywhere"/>
< /configSections>
  </code>
  Add this Section To your Configuration File too
  <code>
< XDavConfig Name="xdav">
    < FileLocation URL="xdav" PathType="Local"></FileLocation>
< /XDavConfig>
  </code>
    </pre>
    
 Name="xdav" <br>

This is a key that couses to xDav module find the WebDAV requests, it means when your request contains '/xdav/filename.docx', that requests handle with xDav module.

<br>
When you set PathType as "Local" it means you have a foldet in your root web Folder with "URL" name,
<br>
<code>
< FileLocation URL="xdav" PathType="Local" >
</code>

And when you set PathType as "Server" you have enter full Server path in "URL" like:
<br>
<code>
FileLocation URL="c:\webdav" PathType="Local" 
</code>
<pre>
And you should Add module settings to your Wen.Config :

<code>
 System.webServer >
    modules>
     add name="XDav" type="XDav.XDavModule, XDav"/>
  /modules>
   /system.webServer>
  </code>
</pre>
  
  Use:
--
<pre>
install the package from nuget:

PM> Install-Package XDav
</pre>
  
