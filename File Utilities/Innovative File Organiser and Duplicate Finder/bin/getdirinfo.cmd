chcp 65001 
echo getting file info  
dir /s /-C /A:-D-H-S  %1 > %2
