<!DOCTYPE html>
<html>
<head>
  <meta http-equiv="X-UA-Compatible" content="IE=Edge">
  <meta charset="utf-8">
  <title><?= htmlspecialchars($title) ?></title>
  <link rel="stylesheet" href="/default.css">
</head>
<body>
<div id="nav">
  <img src="/logo.png"
       height="150"
       width="150"
       alt="Lanscan logo" />
  <ul>
    <li><a href="/">Home</a></li>
    <li><a href="/privacy">Privacy</a></li>
    <li><a href="/release-notes">Release notes</a></li>
  </ul>
</div>
<div id="content">
<h1><?= htmlspecialchars($title) ?></h1>
<?= $contentHtml ?>
</div>
<div id="footer">
  Copyright &copy; 2013&ndash;2018, Richard Cook. All rights reserved.
</div>
</body>
</html>
