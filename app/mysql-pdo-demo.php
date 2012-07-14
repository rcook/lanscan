<?php

define('DB_HOST', 'localhost');
define('DB_NAME', 'monocuit_co_uk');
define('DB_USER', 'jcook');
define('DB_PASSWORD', 'jamesjamesjames');

final class DB {
  private $db;

  public static function openMySqlDB($host, $name, $user, $password) {
    $db = new PDO(
      sprintf('mysql:host=%s;dbname=%s', $host, $name),
      $user,
      $password
    );
    return new DB($db);
  }

  // Do not rely on rowCount method (see http://www.php.net/manual/en/pdostatement.rowcount.php).
  public final function queryWithCountSafe($countSql, $selectSql) {
    $this->db->beginTransaction();
    $rowCount = $this->db->query($countSql)->fetchColumn();
    $rows = $rowCount > 0 ? $this->db->query($selectSql) : array();
    $this->db->commit();
    return array(
      'rowCount' => $rowCount,
      'rows' => $rows
    );
  }

  private final function __construct(PDO $db) {
    $this->db = $db;
  }
}

$db = DB::openMySqlDB(DB_HOST, DB_NAME, DB_USER, DB_PASSWORD);
$result = $db->queryWithCountSafe(
  'SELECT COUNT(*) FROM people',
  'SELECT first_name, last_name FROM people ORDER BY last_name, first_name'
);

?>
<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <title>People</title>
</head>
<body>
<h1>People</h1>
<? if ($result['rowCount'] == 0): ?>
<p>There are no people in the system.</p>
<? else: ?>
<table>
<? foreach ($result['rows'] as $person): ?>
  <tr><td><?= htmlspecialchars($person['last_name']) ?></td><td><?= htmlspecialchars($person['first_name']) ?></td></tr>
<? endforeach ?>
<? endif ?>
</table>
</body>
</html>

