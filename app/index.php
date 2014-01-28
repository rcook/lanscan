<?
$title = 'Lanscan';
$contentHtml = <<<END
<p>
  This is the home of Lanscan, a simple network scanning tool available in the Windows Store for Windows 8, Windows 8.1 and Windows RT.
</p>
<p>
  Features currently include:
</p>
<ul>
  <li>TCP and UDP port scanning</li>
  <li>Scans a predefined list of commonly-used TCP and UDP ports</li>
  <li>Allows user to customize set of TCP and UDP ports to scan</li>
  <li>Provides a readout of essential networking information, including:
    <ul>
      <li>Local IPv4 address</li>
      <li>Network mask and IPv4 address range for current network</li>
      <li>DHCP information including gateway address, DNS server address and domain name</li>
      <li>External IPv4 address</li>
    </ul>
  <li>Performs reverse DNS lookup of host domain name for each detected
  IPv4 endpoint in the network</li>
</ul>
<p>
  I don&rsquo;t have time to actively develop this project, so I thought
  I&rsquo;d share my code with the world! If I&rsquo;m able to fix bugs or
  extend existing features, or if I get good-quality pull requests, I am happy
  to publish updates of the app to the Windows Store from to time.
</p>
<p>
  <a href="http://apps.microsoft.com/windows/app/lanscan/23324308-b07d-471b-b6d5-24ac9a0bb595">Click here to install this app from the Windows Store</a>
</p>
<p>
  <a href="/privacy">Click on this link to read our privacy policy.</a>
</p>
END;
require 'template.php';

