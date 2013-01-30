<?
$title = 'Lanscan release notes';
$contentHtml = <<<END
<h2>Upcoming release 3 (v1.3.0.6, submitted 2013/1/29)</h2>
<h3>Bug fixes</h3>
<ul>
  <li>Display first DNS server address for networks with multiple DNS servers instead of erroring out</li>
  <li>Tweaked theme colours to meet minimum accessibility requirements for colour contrast ratios</li>
</ul>
<h2>Release 2 (v1.3.0.4 2013/1/27)</h2>
<h3>Bug fixes</h3>
<ul>
  <li>Switched on UDP port scanning!</li>
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
  <li>Added protocol readout to network map</li>
</ul>
<h2>Release 1 (v1.1.0.2 2013/1/18)</h2>
<p>
  Lanscan is a simple network and network service discovery tool: it identifies your
  computer's default IPv4 network and scans all hosts in the usable address range for
  available TCP ports for a set of common network services such as port 22 (SSH), port
  80 (HTTP), port 443 (HTTPS) etc. As it completes the scan, it produces a list of all
  discovered end points in your local area network. HTTP/HTTPS end points can be
  clicked which will point your default web browser at the web server in question.
  Lanscan is a tool used to produce a simple map of the current user's local area
  network.
</p>
END;
require 'template.php';

