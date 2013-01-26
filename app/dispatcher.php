<?
$requestUri = $_SERVER['REQUEST_URI'];
if ($requestUri == '/') {
  require_once 'index.php';
  exit(0);
}
elseif ($requestUri == '/default.css') {
  require_once 'default.php';
  exit(0);
}
elseif ($requestUri == '/privacy') {
  require_once 'privacy.php';
  exit(0);
}
elseif ($requestUri == '/release-notes') {
  require_once 'release-notes.php';
  exit(0);
}

header('HTTP/1.0 404 Not Found');

