Get-ChildItem -Recurse -Directory bin | Foreach-Object {
  $_.Delete($true)
}
Get-ChildItem -Recurse -Directory obj | Foreach-Object {
  $_.Delete($true)
}
