# 外部需要传 包名 + token + webhook token
if [ $# != 3 ] ; then 
	echo "error: need three parameters"
	exit 1
fi 

TARGET_PATH="/Users/huge/work/package/iOS"

fir login -T ${2}

fir publish ${TARGET_PATH}/${1}.ipa "--wxwork-webhook=${3}"
