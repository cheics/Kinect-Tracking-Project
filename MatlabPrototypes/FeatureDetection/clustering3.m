function [GEDscore,KNNscore,A] = clustering3(data, testPoses)

good = find (data(:,1) == 1);
Tgood = good;

mG = mean(data(Tgood,2:4));
covG = cov(data(Tgood,2:4));

badB = find (data(:,1) == 2);
TbadB = badB;

mBB = mean(data(TbadB,2:4));
covBB = cov(data(TbadB,2:4));

badH = find (data(:,1) == 3);
TbadH = badH;

mBH = mean(data(TbadH,2:4));
covBH = cov(data(TbadH,2:4));

A = testPoses;
R = [data(Tgood,2:4); data(TbadB,2:4) ; data(TbadH,2:4)];


GEDscore = zeros(length(A),1);
KNNscore = zeros(length(A),1);

for i = 1 : length(A)
    GEDscore(i) = GEDClassifier2(A(i,:),mG,mBB,mBH,covG,covBB,covBH);
    IDX = knnsearch(A(i,:),R);
    if IDX <= length(good)
        KNNscore(i) = 1;
    elseif IDX > (length(good) + length(badB))
        KNNscore(i) = 2;
    else
        KNNscore(i) = 3;
    end
end

%makeClassPlots(GEDscore,data([Egood;EbadB;EbadH],2:4));
%makePlots( goodData, badBData, badNData);

