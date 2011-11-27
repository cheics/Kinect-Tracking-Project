function [score ] = clustering2( goodData, badBData, badNData)

mG = mean(goodData');
covG = cov(goodData');
sdx = std(goodData');

ngoodData = zeros(size(goodData));

for i=1:size(goodData,2)
    ngoodData(:,i)=(goodData(:,i))./(sdx'+(sdx'==0));
end

mBB = mean(badBData');
covBB = cov(badBData');
sdx = std(badBData');

nbadBData = zeros(size(badBData));

for i=1:size(badBData,2)
    nbadBData(:,i)=(badBData(:,i))./(sdx'+(sdx'==0));
end

mBN = mean(badNData');
covBN = (cov(badNData'));
sdx = std(badNData');

nbadNData = zeros(size(badNData));

for i=1:size(badNData,2)
    nbadNData(:,i)=(badNData(:,i))./(sdx'+(sdx'==0));
end
A = [ngoodData nbadBData nbadNData]';
%label = str(length(A));
score = zeros(length(A),1);
for i = 1 : length(A)
    [score(i)] = GEDClassifier2(A(i,:),mG,mBB,mBN,covG,covBB,covBN);
end
makeClassPlots(A,score);
makePlots( goodData, badBData, badNData);