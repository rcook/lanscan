<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <title>Lanscan</title>
  <link rel="stylesheet" href="/default.css">
</head>
<body>
<div id="logo">
  <img src="/static/Logo.scale-100.png"
       height="150"
       width="150"
       alt="Lanscan logo" />
</div>
<h1>Release notes</h1>
<h2>Release 1</h2>
<h2>Release 2</h2>
<p>Bug fixes:</p>
<ul>
  <li>Obtain CIDR/prefix length from system network information instead of inferring it from IP address range</li>
  <li>Removed individual endpoint trace during scan as it hobbled responsiveness of UI</li>
</ul>
<p>Features:</p>
<ul>
  <li>Added gateway address to network info display</li>
  <li>Added DNS server address to network info display</li>
  <li>Added domain name to network info display</li>
  <li>Implemented reverse DNS to resolve IP addresses to FQDNs using local DNS server</li>
  <li>Changed selection mode of network map control to "none" since there is no point in allowing selection: click is still supported</li>
  <li>Perform external IP address lookup using http://whatismyipaddress.com/ and http://checkip.dyndns.org/</li>
  <li>Added app bars and cleaned up UI</li>
</ul>
<? require_once 'footer.php'; ?>
</body>
</html>

