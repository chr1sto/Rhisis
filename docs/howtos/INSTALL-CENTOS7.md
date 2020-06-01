# Installing Rhisis on CentOS 7

## Table of contents
<!--ts-->
* [Installing Rhisis on Windows 10](https://github.com/Eastrall/Rhisis/blob/develop/docs/howtos/README.md)
* Installing Rhisis on CentOS 7
   * [Basic things you want to do in your Linux before we start](#Basic-things-you-want-to-do-in-your-Linux-before-we-start)
   * [Installing firewall and setting up ip forwarding](#Installing-firewall-and-setting-up-ip-forwarding)
   * [Installing .net core 3.1](#Installing-.net-core)
   * [Installing MySQL (MariaDB)](#Installing-MySQL-MariaDB)
   * [Creating a user and table in MySQL for Rhisis](#Creating-a-user-and-table-in-MySQL-for-Rhisis)
   * [Installing PHP, phpMyAdmin and Apache(HTTPD)](#Installing-PHP-phpMyAdmin-and-ApacheHTTPD)
   * [Configuring phpMyAdmin](#Configuring-phpMyAdmin)
   * [Installing Rhisis](#Installing-Rhisis)
<!--te-->
___
## Basic things you want to do in your Linux before we start

1. Let's install some editor so you don't need to use or learn about `vi` :P

```
yum install nano
```

2. Now I usually like having development tools installed in my distro to avoid issues in the event you need to compile or run some specific code:

```
yum group install "Development Tools"
```

## Installing firewall and setting up ip forwarding

1. First let's enable ip forwarding, by simple running the below line:

```
echo 'net.ipv4.ip_forward = 1' >> /etc/sysctl.conf
echo 'net.ipv6.conf.all.forwarding = 1' >> /etc/sysctl.conf
```

2. Now we have to apply the changes by running the following line:

```
/sbin/sysctl -p
```

3. Now let's install firewall-cmd tool to assist us in creating firewall rules:

```
yum install firewalld
```

4. If the above says firewalld is already installed, jump to step 8

5. Now we need to start it:

```
systemctl start firewalld
```

6. Now let's ensure its running:

```
systemctl status firewalld
```
   You should see a green "active (running)" message.

7. Now let's make so it starts when the server boots up:

```
systemctl enable firewalld
```
   You should see an output like:
```
Created symlink from /etc/systemd/system/dbus-org.fedoraproject.FirewallD1.service to /usr/lib/systemd/system/firewalld.service.
Created symlink from /etc/systemd/system/multi-user.target.wants/firewalld.service to /usr/lib/systemd/system/firewalld.service.
```

8. Rhisis will need you to open 4 ports in your firewall, 15400, 23000, 28000, 5400 for tcp, we further need another 2 ports for our http server, 80 and 443, so we need to execute the following commands to make that happen:

```
firewall-cmd --zone=public --add-port=80/tcp --permanent
firewall-cmd --zone=public --add-port=443/tcp --permanent
firewall-cmd --zone=public --add-port=5400/tcp --permanent
firewall-cmd --zone=public --add-port=15400/tcp --permanent
firewall-cmd --zone=public --add-port=23000/tcp --permanent
firewall-cmd --zone=public --add-port=28000/tcp --permanent
```

9. Now we need to restart it to apply the modifications:

```
systemctl restart firewalld
```

## Installing .net core 3.1

1. First we install the repository for yum to use:

```
rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
```

2. Now we run the yum command to install it:

```
yum install dotnet-sdk-3.1
```

3. Now let's check its properly installed by running the version check command:

```
dotnet --list-sdks
```

4. The above should return something like this based on the version you have installed:

```
3.1.101 [/usr/share/dotnet/sdk]
```

# Installing MySQL (MariaDB)

1. On CentOS 7 we have mariadb by default, which is a variation of MySQL, so we will go with that:

```
yum install mariadb-server
```

2. Now let's start it up:

```
systemctl start mariadb
```

3. Now let's ensure its running:

```
systemctl status mariadb
```

   You should see a green "active (running)" message and the last line would look something like this "Jan 15 09:41:12 your hostname systemd[1]: Started MariaDB database server.".
   
4. Now let's make so it starts when the server boots up:

```
systemctl enable mariadb
```

   You should see an output like "Created symlink from /etc/systemd/system/multi-user.target.wants/mariadb.service to /usr/lib/systemd/system/mariadb.service."

5. Now let's change the root password and secure it:

```
    mysql_secure_installation
```

  1. It will ask you for the current password for root, which is always emtpy so just press ENTER to continue.
  2. Now it will ask you, if you want to set the root password, so press Y and enter as we want to do that.
  3. Now it will ask you if you want to remove anonymous users, if this server is already live and running then you should do it(then press Y for yes), otherwise you can leave as is.
   The anonymous user is mainly for testing like sometimes you install a module for mysql and it have a test case, that test case will use the anonymous user for it.
  4. Now it will ask you if you want root to only be allowed access locally, and yes we want that, its not recommended to let the MySQL open specially with root user access for remote connection.
  5. Now it will ask your about test database, its the same as 5.3.
  6. Now it will ask if you want to reload the privileges, thus saving all the changes we opted above.

6. Let's test if all went well by checking the version of our MySQL server:

```
mysqladmin -u root -p version
```

   It will prompt you for your password and you will then get an output like this:

```
mysqladmin  Ver 9.0 Distrib 5.5.64-MariaDB, for Linux on x86_64
Copyright (c) 2000, 2018, Oracle, MariaDB Corporation Ab and others.

Server version          5.5.64-MariaDB
Protocol version        10
Connection              Localhost via UNIX socket
UNIX socket             /var/lib/mysql/mysql.sock
Uptime:                 9 min 17 sec

Threads: 1  Questions: 25  Slow queries: 0  Opens: 1  Flush tables: 2  Open tables: 27  Queries per second avg: 0.044
```

## Creating a user and table in MySQL for Rhisis

1. First, let's connect to MySQL:

```
mysql -u root -p
```

2. Now let's create the database:

```
CREATE DATABASE rhisis DEFAULT CHARACTER SET UTF8 COLLATE utf8_general_ci;
```

3. Now let's add a user to this database(you can use root if you prefer, but I rather make an user for it)

```
GRANT ALL PRIVILEGES ON rhisis.* to 'rhisis'@'localhost' IDENTIFIED BY 'password of your choice';
```

4. Now we update to save the changes and have it working with:

```
FLUSH PRIVILEGES;
```

## Installing PHP, phpMyAdmin and Apache(HTTPD)

1. First we will have to install the red hat repository as phpMyAdmin is not available in the CentOS ones:

```
yum install epel-release
```

2. Now we can install PHP, phpMyAdmin and Apache(AKA httpd):

```
yum install httpd php phpmyadmin
```

3. Now let's start apache:

```
systemctl start httpd
```

4. Now let's check if its running:

```
systemctl status httpd
```
   You should see a green "active (running)" message and the last line would look something like this "Jan 15 09:58:09 your hostname systemd[1]: Started The Apache HTTP Server.".

5. Now let's make so it starts when the server boots up:

```
systemctl enable httpd
```
   You should see an output like "Created symlink from /etc/systemd/system/multi-user.target.wants/httpd.service to /usr/lib/systemd/system/httpd.service.".

6. You should also be able to access your httpd server from a browser now by accessing either your hostname or ip in say firefox or chrome and it should display a testing page from apache.

## Configuring phpMyAdmin

1. Now we can edit the phpMyAdmin config file:

```
nano /etc/httpd/conf.d/phpMyAdmin.conf
```

In the config file, we want to modify this block:
```
   <IfModule mod_authz_core.c>
     # Apache 2.4
     <RequireAny>
       Require all granted
       Require ip 127.0.0.1
       Require ip ::1
     </RequireAny>
   </IfModule>
   <IfModule !mod_authz_core.c>
     # Apache 2.2
     Order Allow,Deny
     Allow from All
     Allow from 127.0.0.1
     Allow from ::1
   </IfModule>
```
Into this:
```
<IfModule mod_authz_core.c>
  # Apache 2.4
  <RequireAny>
    Require ip 127.0.0.1
    Require ip ::1
    # Added this line with your ip http://checkip.dyndns.com/
    Require ip xxx.xxx.xxx.xxx
  </RequireAny>
</IfModule>
<IfModule !mod_authz_core.c>
  # Apache 2.2
  Order Deny,Allow
  Deny from All
  Allow from 127.0.0.1
  Allow from ::1
  # Added this line with your ip http://checkip.dyndns.com/
  Allow from xxx.xxx.xxx.xxx
</IfModule>
```
This will allow so that your IP can access it.

2. Now we restart apache and we are done:

```
systemctl restart httpd
```

3. You can now access phpMyAdmin from http://hostname/phpmyadmin or http://your_server_ip/phpmyadmin

## Installing Rhisis

1. First let's create a user to run it from:

```
useradd -m rhisis
```

2. Now let's login into that user

```
su - rhisis
```

3. Add a environment variable to disable DOTNET telemetry so that compiling goes slightly faster by not having to send out data to microsoft.

```
echo 'export DOTNET_CLI_TELEMETRY_OPTOUT=1' >> $HOME/.bashrc
```

4. The above will work in your next login, so let's export it now for this session:

```
export DOTNET_CLI_TELEMETRY_OPTOUT=1
```

5. Now let's clone rhisis project

```
git clone  --recursive https://github.com/Eastrall/Rhisis.git
```

6. Now let's move into the repository we just cloned

```
cd Rhisis
```

7. Now let's build it:

```
dotnet build
```
   NOTE: Depending on your server spec this might take awhile, on a 1vCPU with 2GB it took roughly 10 seconds.

8. Now let's configure the server, by first entering in the output folder of our build:

```
cd bin
```

9. First let's make rhisis-cli executable:

```
chmod +x rhisis-cli
```

10. Now let's initialize the database settings:

```
./rhisis-cli database initialize
```
1. It will ask you the MySQL IP, if the MySQL server is in the same machine, use `127.0.0.1`, otherwise put the IP where its located.
2. MySQL default port is `3306`, if you have changed it then you need to put the port in there.
3. You can use root or another user you may have created specifically for this.
4. Your user password
5. The database name you have created for rhisis
6. `Use encryption?` by replying with y/yes it will encrypt all the data inside your database to comply with GDPR.

11. Now let's create an account:

```
./rhisis-cli user create
```
  1. Fill in the username, email, password and confirm your password, when it asks you for the salt, use `kikugalanet`

12. To setup the login, cluster and world server, run the following commands:

```
./rhisis-cli configure login
./rhisis-cli configure cluster
./rhisis-cli configure world
```

13. Congratulations, you're ready to run your own FlyFF server!

14. First let's create a folder to store the log files:

```
mkdir logs
```

15. Now let's run the login server:

```
dotnet run --project ../src/Rhisis.Login/Rhisis.Login.csproj --configuration Debug > logs/Login.log 2>&1 &
sleep 25
```

16. Now let's run the cluster server:

```
dotnet run --project ../src/Rhisis.Cluster/Rhisis.Cluster.csproj --configuration Debug > logs/Cluster.log 2>&1 &
sleep 25
```

17. Now let's run the world server:

```
dotnet run --project ../src/Rhisis.World/Rhisis.World.csproj --configuration Release > logs/World.log 2>&1 &
sleep 25
```

18. The sleep 25, is so that each server can load and connect to the core server without fail due to being in the process of loading it self.

19. Now you can connect to your server and enjoy! Have fun!