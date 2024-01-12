# 外部需要传 工程的路径 + 包名
if [ $# != 5 ] ; then 
	echo "error: need 5 parameters"
	exit 1
fi 

TARGET_PATH="/Users/huge/work/package/iOS"
PROJECT_PATH=${1}
PACKAGE_NAME=$(echo "${2}" | tr -d ' ') #Avatar
PACKAGE_TYPE=${3} #development、ad-hoc、app-store、enterprise
DEBUG_OR_RELEASE=${4} #Debug、Release
TEAM_ID=${5} #xxxxxx

cd ${PROJECT_PATH}

echo "ipa clean"
echo ${PACKAGE_NAME}
rm ${TARGET_PATH}/${PACKAGE_NAME}.ipa

echo "xcodebuild clean -quiet"
rm -rf *.xcarchive
rm -rf ~/Library/Developer/Xcode/DerivedData/*
xcodebuild clean

echo "pod update"
export https_proxy=http://127.0.0.1:7890 http_proxy=http://127.0.0.1:7890 all_proxy=socks5://127.0.0.1:7890
pod install --verbose

ExportOptionsPlist=${PROJECT_PATH}/ExportOptionsPlist.plist
rm $ExportOptionsPlist

echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>">>$ExportOptionsPlist
echo "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">">>$ExportOptionsPlist
echo "<plist version=\"1.0\">">>$ExportOptionsPlist
echo "<dict>">>$ExportOptionsPlist
echo "		<key>compileBitcode</key>">>$ExportOptionsPlist
echo "		<false/>">>$ExportOptionsPlist
echo "		<key>method</key>">>$ExportOptionsPlist
echo "		<string>${PACKAGE_TYPE}</string>">>$ExportOptionsPlist
echo " 		<key>signingStyle</key>">>$ExportOptionsPlist
echo "		<string>automatic</string>">>$ExportOptionsPlist
echo "		<key>stripSwiftSymbols</key>">>$ExportOptionsPlist
echo "		<true/>">>$ExportOptionsPlist
echo "		<key>teamID</key>">>$ExportOptionsPlist
echo "		<string>${TEAM_ID}</string>">>$ExportOptionsPlist
echo "		<key>thinning</key>">>$ExportOptionsPlist
echo " 		<string>&lt;none&gt;</string>">>$ExportOptionsPlist
echo "</dict>">>$ExportOptionsPlist
echo "</plist>">>$ExportOptionsPlist

echo "xcodebuild archive"
xcodebuild archive -quiet \
		-workspace Unity-iPhone.xcworkspace \
		-scheme Unity-iPhone \
		-archivePath Unity-iPhone.xcarchive \
		-configuration DEBUG_OR_RELEASE \
		-destination generic/platform=iOS \
		DWARF_DSYM_FOLDER_PATH=$(PWD) \
		CODE_SIGN_STYLE="Automatic" \
		DEVELOPMENT_TEAM=${TEAM_ID}

echo "export ipa"
xcodebuild -quiet -exportArchive -archivePath Unity-iPhone.xcarchive -exportPath ${PROJECT_PATH}/ipa  -exportOptionsPlist ${ExportOptionsPlist} -allowProvisioningUpdates

OUTPUT=${PROJECT_PATH}/ipa

if [ ! -d "$OUTPUT" ]; then  
	echo "error: not found build folder"
	exit 1
fi

cd ipa

mv ${PACKAGE_NAME}.ipa ${TARGET_PATH}/${PACKAGE_NAME}.ipa
