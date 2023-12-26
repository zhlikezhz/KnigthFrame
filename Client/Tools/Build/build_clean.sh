# 外部需要传 工程的路径 + 分支名
if [ $# != 2 ] ; then 
echo "参数个数不对"
exit 1
fi 

cd ${1}

git prune

git remote prune origin

git fetch -p

git clean -dfq

git checkout -q .

git checkout ${2}

git pull

git submodule foreach git prune

git submodule foreach git remote prune orgin

git submodule foreach git fetch -p

git submodule foreach git clean -dfq

git submodule foreach git pull

echo "current branch"
git branch

echo "current log"
git log -1