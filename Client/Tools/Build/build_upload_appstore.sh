# 外部需要传 包名 + username + password
if [ $# != 3 ] ; then 
	echo "error: need 3 parameters"
	exit 1
fi 

TARGET_PATH="/Users/huge/work/package/iOS"
PACKAGE_NAME=$(echo "${1}" | tr -d ' ') #Avatar
PACKAGE_PATH=${TARGET_PATH}/${PACKAGE_NAME}.ipa
xcrun altool --validate-app -f ${PACKAGE_PATH} -t ios -u ${2} -p ${3}


if [ $? -eq 0 ]; then
    xcrun altool --upload-app -f ${PACKAGE_PATH} -t ios -u ${2} -p ${3}
    exit 0
else
    echo "error: upload ipa fail"
    exit 1
end