critComp=1;
useFeatures=[2,4,5];


% gridSize=0.5;
% xVec=round(min(trainingData.features(:,2))):gridSize:round(max(trainingData.features(:,2)));
% yVec=round(min(trainingData.features(:,4))):gridSize:round(max(trainingData.features(:,4)));
% 
% 
% F = TriScatteredInterp(trainingData.features(:,2), trainingData.features(:,4), trainingData.features(:,5), 'natural');
% [qx,qy] = meshgrid(xVec,yVec);
% qz = F(qx,qy);
%contour(qx,qy,qz);
%hold on

critCompScore=trainingData.classes(:,critComp);
i2=find(critCompScore==2); i1=find(critCompScore==1); i0=find(critCompScore==0);
plot3(trainingData.features(i2,useFeatures(1)), trainingData.features(i2,useFeatures(2)), trainingData.features(i2,useFeatures(3)), 'g.',...
	trainingData.features(i1,useFeatures(1)), trainingData.features(i1,useFeatures(2)), trainingData.features(i1,useFeatures(3)),'b.',...
	trainingData.features(i0,useFeatures(1)), trainingData.features(i0,useFeatures(2)), trainingData.features(i0,useFeatures(3)),'r.')