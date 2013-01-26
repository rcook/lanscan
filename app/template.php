<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <title><?= htmlspecialchars($title) ?></title>
  <link rel="stylesheet" href="/default.css">
</head>
<body>
<div id="nav">
  <img src="/static/Logo.scale-100.png"
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
<? require_once 'footer.php'; ?>
</body>
</html>
