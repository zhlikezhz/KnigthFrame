# 外部需要传 包名 + token + webhook token
if [ $# != 3 ] ; then 
	echo "error: need three parameters"
	exit 1
fi 

TARGET_PATH="/Users/huge/work/package/iOS"
PACKAGE_NAME=$(echo "${1}" | tr -d ' ') #Avatar

fir login -T ${2}

fir publish ${TARGET_PATH}/${PACKAGE_NAME}.ipa "--wxwork-webhook=${3}"
