# Lanscan

A simple network scanning tool for Windows Store

* [Home page](http://lanscan.rcook.org/)
* [Privacy policy](http://lanscan.rcook.org/privacy)
* [Release notes](http://lanscan.rcook.org/release-notes)
* [Windows Store home page](http://apps.microsoft.com/windows/en-us/app/lanscan/23324308-b07d-471b-b6d5-24ac9a0bb595)

Features currently include:

* TCP and UDP port scanning
* Scans a predefined list of commonly-used TCP and UDP ports
* Allows user to customize set of TCP and UDP ports to scan
* Provides a readout of essential networking information, including:
 * Local IPv4 address
 * Network mask and IPv4 address range for current network
 * DHCP information including gateway address, DNS server address and domain
name
 * External IPv4 address
* Performs reverse DNS lookup of host domain name for each detected IPv4
endpoint in the network

I don't have time to actively develop this project, so I thought I'd share my
code with the world! If I'm able to fix bugs or extend existing features, or if
I get good-quality pull requests, I am happy to publish updates of the app to
the Windows Store from to time.

## Contributing

* Please submit clean pull requests clear of unrelated changes
* Please ensure current unit tests continue to pass
* Please add new unit tests for new functionality wherever possible
* Please observe existing coding conventions:
 * Use Windows line endings for C# source files
 * Format C# source files using Visual Studio's default code formatting settings
 * `using` statements inside namespaces, please!
* Please observe existing naming conventions:
 * Upper camel case for namespaces, classes, methods, properties etc.
 * Lower camel case for local variables, method parameters
 * Instance member variables use `m_` prefix and lower camel case
 * Static member variables use `s_` prefix and lower camel case
 * All the usual stuff for C#/.NET code
* A note about my private branches:
 * I name my private branches with a `p-rcook-` prefix
 * I extensively rebase and edit history on private branches
 * Feel free to pull from these branches, but don't be surprised if I randomly
rebase them underneath you!

## Licence

Lanscan is released under the MIT licence. Pursuant to this licence, any person
may fork this project and publish their own Windows Store (or other) app based
on the source code contained herein subject to the conditions of the licence:
the copyright notice must be included in all copies or substantial portions of
the software.

