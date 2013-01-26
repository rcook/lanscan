<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <title><$= htmlspecialchars($title) ?></title>
  <link rel="stylesheet" href="/default.css">
</head>
<body>
<div id="logo">
  <img src="/static/Logo.scale-100.png"
       height="150"
       width="150"
       alt="Lanscan logo" />
</div>
<div id="content">
<?= $contentHtml ?>
</div>
<? require_once 'footer.php'; ?>
</body>
</html>
