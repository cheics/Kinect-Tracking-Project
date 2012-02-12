load('trainingData.mat');

critComp=1;
useFeatures=[2,4,5];

contourFind(trainingData, critComp, useFeatures);
