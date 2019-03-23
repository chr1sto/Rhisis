#!/bin/bash

#
# Author: Filipe GOMES PEIXOTO <gomespeixoto.filipe@gmail.com>
# Title: Rhisis setup script
# Description :
# This script is going to install the Rhisis emulator in the /var/rhisis directory.
# You can specify the directory by doing:
# $> ./rhisis-install [directory]
#

#
# Global variables
#
REPOSITORY="https://github.com/Eastrall/Rhisis"
INSTALL_DIRECTORY="/var/rhisis"
MYSQL_ROOT_PASSWORD="password_root"
RHISIS_CLI_COMMAND="/usr/bin/rhisis-cli"

#
# Update, updgrade and install packages.
#
function install_tools {
	sudo apt-get udpate
	sudo apt-get upgrade
	sudo apt-get install -y git curl libunwind8 gettext apt-transport-https
}

#
# Check and install MySQL if it is not installed yet.
#
function install_mysql {
	type mysql >/dev/null 2>&1 && MYSQL_INSTALLED=1 || MYSQL_INSTALLED=0

	if [ "$MYSQL_INSTALLED" == 0 ]; then
		# prepare root user
		export DEBIAN_FRONTEND="noninteractive"
		sudo debconf-set-selections <<< "mysql-server mysql-server/root_password password $MYSQL_ROOT_PASSWORD"
		sudo debconf-set-selections <<< "mysql-server mysql-server/root_password_again password $MYSQL_ROOT_PASSWORD"

		# install mysql server and client
		sudo apt-get install -y mysql-server mysql-client
		sudo /etc/init.d/mysql stop # stop the service
		sudo mysql -u "root" "-p$MYSQL_ROOT_PASSWORD" --bind-address="0.0.0.0"
		sudo /etc/init.d/mysql start # start the service
	fi
}

#
# Install .NET Core SDK 2.0
#
function install_dotnet_core {
    CODENAME=`lsb_release --codename | cut -f2`
    
    # Register Microsoft trusted key
    curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
	sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
	
	sudo echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-debian-$CODENAME-prod $CODENAME main" > /etc/apt/sources.list.d/dotnetdev.list
	
	sudo apt-get update
	sudo apt-get install dotnet-sdk-2.0.2
}

# Check install directory

if [ ! -z $1 ]; then
    INSTALL_DIRECTORY=$1
fi

if [ -d "$INSTALL_DIRECTORY" ]; then
    while true; do
	read -p "'$INSTALL_DIRECTORY' already exists, do you want to delete it? (y/n) " yn
	case $yn in
	    [Yy]* )
		# Remove current install directory
		echo "Deleting '$INSTALL_DIRECTORY'..."
		rm -rf $INSTALL_DIRECTORY

		# Stop the existing services
		sudo service rhisis-login stop
		sudo service rhisis-cluster stop
		sudo service rhisis-world stop
		break
		;;
	    [Nn]* ) exit;;
	    * ) echo "Please answer yes or no.";;
	esac
    done
fi

# Install tools
install_tools
install_mysql
install_dotnet_core

# Install Rhisis emulator
sudo git clone $REPOSITORY
cd Rhisis/

# Start build dist script
sudo chmod +x ./build/build-dist.sh
sudo chmod +x ./build/build.sh
sudo ./build/build-dist.sh $INSTALL_DIRECTORY

# Move dist files to install directory
mkdir $INSTALL_DIRECTORY
cp -a dist/. $INSTALL_DIRECTORY

# Copy service files
cp script/debian/rhisis-login /etc/init.d/
cp script/debian/rhisis-cluster /etc/init.d/
cp script/debian/rhisis-world /etc/init.d/
sudo chmod +x /etc/init.d/rhisis-*
sudo systemctl daemon-reload

# Create symlink for Rhisis CLI
sudo rm -rf $RHISIS_CLI_COMMAND
sudo ln -s $INSTALL_DIRECTORY/rhisis-cli $RHISIS_CLI_COMMAND

# Delete Rhisis repository
cd ..
sudo rm -rf Rhisis/