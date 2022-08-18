# Tech Log #

*Log of my work, tools and more*

## Projects

### Cryptography ###

#### Enabling Bitlocker with pre-provisioned Encryption in the  installers. ####

In a regulated environments like health care, data theft such as patient related can be a costly ordeal. Disk encryption helps to safeguard the data when it reaches wrong hands.
In Windows environment, bitlocker enables full volume encryption. When coupled with TPM, it's almost ensured that the data will be accessible on the same hardware.

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/enabling-bitlocker-with-pre-provisioned.html)

-------------------------------------------------------

### COM+ ###

#### APITester - Test any COM component using its typelibrary  ####

COM components are often used in backend, require elaborate workflows, complex GUIs. For ad hoc testing at for example customer site with specific inputs, separate test application needs to be created. APITester tries to solve this problem, It reads the typelibrary and presents a simple user interface that lists  exposed apis and their parameters 

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/apitester-test-any-com-component-using.html)

#### Late Binding Client Code Generator for COM Components ####

Presently .NET framework provides RCW (Remote callable wrapper) mechanism for .NET code to interface with COM components. However it does not support late binding requred for scripting.

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/late-binding-client-code-generator-for.html)

#### Working with Managed COM Component ####

COM components can also be generated under .net environment using Visual studio and languages such as C#. 

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/working-with-managed-com-component.html)

#### Working with WSC (Windows scripting component) ####

Windows scripting component or WSC provide an easier javascript or vscript based solution for creating COM components  using command line tools. WSC are essentially XML based  that can be used to generate COM based object that can even support events.

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/working-with-wsc-windows-scripting.html)

#### Transform COM+ serviced component to WCF Service ####

WCF is widely used in managed software for IPC. However porting existing COM based applications to WCF architecture can be daunting task. A balance can be achieved by deploying existing COM based applications as a WCF service so that it can be accessed by WCF clients.

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/transform-com-serviced-component-to-wcf.html)

-------------------------------------------------------

### Database ###

#### ISQL - Query, Manage any ODBC based data source ####

Open Database Connectivity or ODBC is a part of the windows OS that enables accessing data from different data sources using a single interface. For example, RDBMS, text files, excel files etc. using Structured Query Language or SQL. 
There are no tools available by the OS to query using SQL.

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/isql-query-manage-any-odbc-based-data.html)

-------------------------------------------------------

### Virtualization ###

#### Desktop Virtualization of Released Software using Virtual Hard Disk ####

Software Applications in regulated domains such as health care use customised  OS based  on an embedded OS. 
Such application software installations are time consuming and older releases require CDs subjected to wear and tear.
A Virtual Hard Disk or  VHD is a flat file  on the host PC that can be mounted as a Hard disk on the system. Further a VHD can be made bootable by adding an entry to the boot menu. 
Desktop virtualization enables running different releases of the hospital software even with different OS without installation. 

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/desktop-virtualization-of-released.html)

#### Using bootable Virtual Hard Drive as Media instead of USB Drive/DVD Disk ####

Installing Hospital software uses various types of media such as CD/DVD or USB. This is mainly because of deployment of custom Operating system. These type of installations cannot be automated because of  need of manual operations such as boot options, bios password etc.
A bootable Virtual Hard Disk( VHD) provides a perfect solution.

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/using-bootable-virtual-hard-drive-as.html)

-------------------------------------------------------

### DevOps ###

#### Digital Code Signing binaries and scripts during Nightly Builds  ####

Digital Code signing binaries such as exes, dlls , scripts by the software supplier not only identifies the source of the software but also asserts authenticity. 
Digital Code signing is usually done as part of the build process where binaries are digitally signed as they are built. 
However this can impose overhead and add complexities in development build environments. 
An ideal solution would be to do digital signing as a separate step during nightly production builds that unpackages binaries from MSI, CAB, Zip files;digitally sign them and repackage back.

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/digital-code-signing-binaries-and.html)

#### Easy MSI Installer Creator with Actions ####

Often times software, data files etc. needs to be packaged and sent across. Zip files are sufficient in most cases. However, sometimes complex deployment needs could arise. For example, multiple versions, operations on the files, folders before copy and after copy etc.
Easy Installer Creator provides an easy MSI based solution.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/easy-msi-installer-creator-with-actions.html)

#### Update an MSI Installer without rebuilding ####

In many development environments, updating outdated Installers of the development tools can be challenging as the original development environment used for generating the installer will be no longer available. 
In such cases, an installer can be disassembled, changes be applied and reassembled back using MS installer APIs.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/update-msi-installer-without-rebuilding.html)

#### Setup for Remote Debugging with Symbol Server and Source Indexing ####

Troubleshooting production issues of an older release is daunting due to non availability of symbols and source code. Symbol Server and Source Indexing technogies make it seamless to debug.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/setup-for-remote-debugging-with-symbol.html)

#### Source Indexer for RTC repository ####

Source Indexing enables downloading source code from source control during debugging in Visual Studio. This is very helpful while debugging production issues of older releases. The current Source Indexing setup from Microsoft doesn't support RTC repository. Writing the same in Perl seems complicated for those unfamiliar with Perl.
RTC repository Source Indexer enables Source indexing RTC Repositories seamlessly.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/source-indexer-for-rtc-repository.html)

#### Visual Studio Migration Tool ####

During a project's life cycle, moving the code base to the next version of visual studio becomes necessary to take advantage of newer features.
This can be daunting task if there are a large number of projects.
Visual Migration Helper utility makes it a breeze.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/visual-studio-migration-tool.html)

#### Truly Portable Patch Tool and file comparer ####

In the diversified development environment of today, multiple code repositories are used by cross development teams. As each have an indigenous way to deal with patches, a generic method of creating and applying patches is much desired along with recursive folder comparison and viewing the file differences and other operations. The portable patch tool attempts to address. 

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/truly-portable-patch-tool-and-file.html)

-------------------------------------------------------

### File Utilities ###

#### A Backup, Restore, and Update Tool with a refined approach ####

Over the period number of files grow, change, removed, copied and moved etc. Keeping track of these will be a challenge unless they are backed up and tracked to restore when needed. 
- Backup archives and their meta data should be transferable, encrypted. 
- Single copy of the file should be stored in the backup archive.
- A backup should not only store new or changed files but also keep links to older files.
- It should be possible to restore backups to a different folder.
- It should be possible to directly edit a backup to add / remove files without having the backup folder.
- and much more.

Read more about it [here](https://techlog-vedavyasarao.blogspot.com/2022/08/a-backup-restore-and-update-tool-with.html)

#### An Innovative File organizer with Duplicates finder and Hierarchical viewer ####

When there is an urgent need to free up space, getting a fair idea of  the storage distribution becomes paramount. Also Multiple copies of the same file such as pictures gets spread across the disk. It also becomes important to keep track from security Point of View.
FileOrganiser tool attempts to address this by 
- Provide a hierarchical  view of the storage distribution along with options to sort data in multiple ways.
- Track duplicate files
- Compare two folders having different folder structures but same files
- Export file names with their CRCs

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/an-innovative-file-organizer-with.html)

#### Script to Zip and Unzip Files using Window shell ####

Often times applications require files to be zipped to a .zip file and extract files from the same. makezip_unzip.vbs  uses Windows shell do this easily.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/script-to-zip-and-unzip-files-using.html)

#### File Splitter and Merger Tool for Transmission ####

It's an command line utility to truncate a big file into small pieces, later to be assembled. Useful for sending big attachment files in mails, copying a big file into floppies etc

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/file-splitter-and-merger-tool-for.html)

-------------------------------------------------------

### Distributed Applications ###

#### Simple Managed Inter Process Communication(IPC) Framework ####

If the need is just to do a cross process communication within the box, it'd be bit heavy to use frameworks such as WCF as it comes with a learning curve and complex setup. However  home grown solutions such as messaged based implementation suffer from flexibility and heavy maintenance.
The simpleIPC framework tries to strike right balance with the interface based programming coupled with zero setup.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/simple-managed-inter-process.html)

#### Use System Cache memory for your applications ####

Windows OS uses Cache memory set aside for faster IO with disk. For example when an user launches an application  for the first time, it's loaded into cache memory. Next time when user launches the same application it loads faster since it's loaded from the cache memory instead from disk. In 32 bit OS such as Win XP , the maximum cache size is about 960 MB and in case of 64 bit like windows 7 it's 1 TB.
User applications can also take advantage of this such that data can be directly written to cache memory and read from there.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/use-system-cache-memory-for-your.html)

#### Share large allocated memory across applications without duplication ####

In some client/server applications running on same box,  a large amount of data may need to be shared. For example, an image acquisition application sharing the image with its clients. This typically involves a RPC mechanism duplicating data. Instead ReadProcessMemory and WriteProcessMemory APIs can be used.
What this means is a Process A can share its array with Process B without doing  IPC except sharing its PID and address of the array. 

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/share-large-allocated-memory-across.html)

#### Ultra fast compression and decompression Win32 APIs for realtime applications ####

Windows OS internally use lightning fast compression and decompression APIs. This can be used by user applications also. 
For example, a 5 MB DICOM image can be compressed to almost 40% in 38 ms and decompressed in 8ms.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/ultra-fast-compression-and.html)

#### Running Custom applications using task scheduler when the system is idle  ####

Many times  it is necessary to run background tasks when the system is idle. In such cases, the task scheduler can be used to schedule a task upon OnIdle condition. When the system goes out of idle say due to an user action, this task is immediately terminated. 
When the on idle task is terminated, it's desirable to take an action. As windows OS provides no direct way to achieve this, it can be implemented by tracking the process termination.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/running-custom-applications-using-task.html)

#### Learning some useful Windows OS Concepts ####

Windows Operating system provides a host of facilities listed below that can be utilized by user applications.
- Overlapped IO
- IO Completion Port
- Windows System cache
- File Mapping backed by NTFS sparse file
- Sharing large byte arrays across processes
- Simple IPC Framework using kernel objects
- Ultrafast Compression and Decompression

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/learning-some-useful-windows-os-concepts.html)

-------------------------------------------------------

### Internationalization ###

#### Supporting Chinese, Japanese and Korean (CJK) languages in MBCS C++ Applications  ####

There are plenty of  MBCS based legacy MFC applications out there that require CJK language support. Migrating to Unicode is not an option in these cases. 
All the resources such as menus, dialogs, string tables used by the application are defined in the .rc file. To support a CJK language, system wide changes should be made and the CJK .rc file must be coded in a certan way.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/supporting-chinese-japanese-and-korean.html)

-------------------------------------------------------

### Network ###

#### Setting and Retrieving  IPv6 Configuration ####

Unlike IPv4, configuring and retrieving IPv6 information on a PC can be daunting. Depending on the configuration, multiple IP address might be returned by an IPv6 host. IPV6Configurator utility retrives and sets IPV6 addresses seamlessly.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/rvv-quotemate-simple-trading-system-for.html)

-------------------------------------------------------

### Stock Market ###

#### RVV Quotemate - A simple trading system for equity markets ####

There are plenty of free web portals for doing research to pick a stock . Example, Yahoo finance, Google Finance, finviz, trading view etc. These provide valuable information such as  real-time quotes, charts, news, institution actions etc. However, it requires hopping to different web browser tabs or windows.
RVV Quotemate attempts to provide an integrated view by creating a split view where top view provides real time quotes, alerts etc for the portfolio and bottom view displays the chosen web portal for the selected stock.

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/rvv-quotemate-simple-trading-system-for.html)

-------------------------------------------------------

### User Interface Automation ###

#### User Interface Automation Framework ####

The Microsoft UI Automation APIs enable navigation of user interfaces programmatically. Some of the areas of application are UI Automation testing,  manufacturing Quality checks, Accessibility etc.
The User Interface Automation Framework is a light weight framework that can be used to achieve UI Automation.either by Scripting languages or C#. 

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/user-interface-automation-framework.html)

#### Unattended System monitoring with ProcessMonitor and UI automation ####

ProcessMonitor from Microsoft is widely used to monitor events such as registry or file related updates. 
If Process monitor is used to track an event  (e.g., a registry change) system wide, in an unattended scenario, some customization will be needed since ProcessMonitor  creates a large amount of log files in a short span of time i.e., around 5gb in 30 minutes. 
Using a task scheduler task and UIA, Process monitor log files can be checked every 30 minutes for the event, take a snapshot and clear the logs. 

Read more about this [here](https://techlog-vedavyasarao.blogspot.com/2022/08/unattended-system-monitoring-with.html)

-------------------------------------------------------








