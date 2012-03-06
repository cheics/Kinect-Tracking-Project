function [ outClass ] = testBayes( tfv )
%UNTITLED Summary of this function goes here
%   tfv:	 test feature vector 11x1
	cc=MakeClassifier('BAYES', false);
	[tf, tc]=getTrainingData('squats', 1);
	cc.trainClassifiers(tf, tc);
	%% outclass is 1x3
	outClass=cc.classify(tfv');
end