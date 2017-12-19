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

#
# Update, updgrade and install packages.
#
function install_tools {
	sudo apt-get udpate && apt-get upgrade -y
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
	# Register Microsoft trusted key
	curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
	sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
	
	sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-debian-jessie-prod jessie main" > /etc/apt/sources.list.d/dotnetdev.list'
	
	sudo apt-get update
	sudo apt-get install dotnet-sdk-2.0.0
}

# Check install directory

if [ ! -z $1 ]; then
    INSTALL_DIRECTORY=$1
fi

if [ -d "$INSTALL_DIRECTORY" ]; then
    while true; do
	read -p "'$INSTALL_DIRECTORY' already exists, do you want to delete it?" yn
	case $yn in
	    [Yy]* )
		echo "Deleting '$INSTALL_DIRECTORY'..."
		rm -rf $INSTALL_DIRECTORY
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
install_dotnet

# Install Rhisis emulator
echo "Clone repository"
git clone $REPOSITORY

echo "change directory"
cd Rhisis/

echo "Give access to script"
chmod +x ./Rhisis/build/build-dist.sh

echo "start script"
./Rhisis/build/build-dist.sh
