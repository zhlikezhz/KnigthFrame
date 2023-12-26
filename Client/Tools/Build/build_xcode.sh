# 外部需要传 工程的路径 + 包名
if [ $# != 2 ] ; then 
	echo "error: need two parameters"
	exit 1
fi 

TARGET_PATH="/Users/huge/work/package/iOS"
EXPORTOPTIONS_PATH="/Users/huge/work/package/config/ExportOptions.plist"

cd ${1}

echo "ipa clean"
rm ${TARGET_PATH}/${2}.ipa

echo "xcodebuild clean -quiet"
rm -rf *.xcarchive
rm -rf ~/Library/Developer/Xcode/DerivedData/*
xcodebuild clean

echo "pod update"
export https_proxy=http://127.0.0.1:7890 http_proxy=http://127.0.0.1:7890 all_proxy=socks5://127.0.0.1:7890
pod install --verbose

echo "xcodebuild archive"
xcodebuild archive -quiet -workspace Unity-iPhone.xcworkspace \
		-scheme Unity-iPhone \
		-archivePath Unity-iPhone.xcarchive  \
		DWARF_DSYM_FOLDER_PATH=$(PWD) \
		CODE_SIGN_STYLE="Manual" \
		CODE_SIGN_IDENTITY="$CODE_SIGN_IDENTITY" \

echo "export ipa"
xcodebuild -quiet -exportArchive -archivePath Unity-iPhone.xcarchive -exportPath ${1}/ipa  -exportOptionsPlist ${EXPORTOPTIONS_PATH}

OUTPUT=${1}/ipa

if [ ! -d "$OUTPUT" ]; then  
	echo "error: not found build folder"
	exit 1
fi

cd ipa

mv ${2}.ipa ${TARGET_PATH}/${2}.ipa