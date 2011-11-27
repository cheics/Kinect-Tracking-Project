function [score ] = clustering( goodData, badBData, badNData)

mG = mean(goodData');
covG = cov(goodData');

% ngoodData = zeros(size(goodData));
% 
% for i=1:size(goodData,2)
%     ngoodData(:,i)=(goodData(:,i))./(sdx'+(sdx'==0));
% end

mBB = mean(badBData');
covBB = cov(badBData');

% nbadBData = zeros(size(badBData));
% 
% for i=1:size(badBData,2)
%     nbadBData(:,i)=(badBData(:,i))./(sdx'+(sdx'==0));
% end

mBN = mean(badNData');
covBN = (cov(badNData'));

% nbadNData = zeros(size(badNData));
% 
% for i=1:size(badNData,2)
%     nbadNData(:,i)=(badNData(:,i))./(sdx'+(sdx'==0));
% end
A = [goodData badBData badNData]';
%label = str(length(A));
score = zeros(length(A),1);
for i = 1 : length(A)
    [score(i)] = GEDClassifier2(A(i,:),mG,mBB,mBN,covG,covBB,covBN);
end
makeClassPlots(A,score);
makePlots( goodData, badBData, badNData);


% X = pdist([ngoodData nbadBData nbadNData]');
% Y = linkage(X,'single');
% 
% H = dendrogram(Y,0);
% 
% cutoff = 0.5;
% correction = 0.005;
% T1 = cluster(Y,'cutoff',cutoff);
% Classes = cluster(Y,'cutoff',cutoff);
% 
% while max(T1) >= 3
%     Classes = cluster(Y,'cutoff',cutoff);
%     cutoff = cutoff + correction;
%     T1 = cluster(Y,'cutoff',cutoff);
%     %correction = correction*correction;
% end
