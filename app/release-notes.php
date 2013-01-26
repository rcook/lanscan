<?
$title = 'Lanscan release notes';
$contentHtml = <<<END
<h2>Release 2 v1.2.0.3 (2013/1/25)</h2>
<h3>Bug fixes</h3>
<ul>
  <li>CIDR/network prefix length now obtained from system network information instead
  of trying to infer it from the device's IP address</li>
  <li>Removed individual endpoint trace during scan as it hobbled responsiveness of UI</li>
  <li>Fixed cancellation behaviour</li>
</ul>
<h3>New features</h3>
<ul>
  <li>Added gateway address to network info display</li>
  <li>Added DNS server address to network info display</li>
  <li>Added domain name to network info display</li>
  <li>Implemented reverse DNS to resolve IP addresses to FQDNs using local DNS server</li>
  <li>Changed selection mode of network map control to "none" since there is no point
  in allowing selection: click for port 80/443 is still supported</li>
  <li>Performs external IP address lookup using <a href="http://whatismyipaddress.com/">http://whatismyipaddress.com/</a>
  and <a href="http://checkip.dyndns.org/">http://checkip.dyndns.org/</a></li>
  <li>Added app bars and cleaned up UI</li>
</ul>
<h2>Release 1 (v1.1.0.2 (2013/1/18)</h2>
<p>
  Initial release.
</p>
END;
require 'template.php';

