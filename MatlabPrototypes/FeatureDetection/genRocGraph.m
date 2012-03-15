% [tf,tc]=getTrainingData('legExt', 1);
% b_acc=LeaveOneOutValid(MakeClassifier('BAYES', false), tf, tc);
% m_acc=LeaveOneOutValid(MakeClassifier('MAP', false), tf, tc);
% s_acc=LeaveOneOutValid(MakeClassifier('SVM', false), tf, tc);
x_label='Reps training data';
y_label='Classifier weighted accuracy';
xlim1=[10,180];
xlim2=[10,400];
g1=quickG(bayes_armRaise_ROC,mapROC_armRaise,svmROC_armRaise, xlim1);
g2=quickG(bayes_legRaise_ROC,mapROC_legRaise,svmROC_legRaise, xlim1);
g3=quickG(bayes_squat_ROC,mapROC_squats,svmROC_squat, [10,400]);
g4=quickG(bayes_legExt_ROC,mapROC_legExt,svmROC_legExt, xlim1);

plot(g1(:,1),g1(:,2),...
	g1(:,1),g1(:,3),...
	g1(:,1),g1(:,4));
title('Arm Raise');
xlabel(x_label);
ylabel(y_label);
xlim([0,xlim1(2)]);
ylim([0,100]);

figure;
plot(g2(:,1),g2(:,2),...
	g2(:,1),g2(:,3),...
	g2(:,1),g2(:,4));
title('Leg Raise');
xlabel(x_label);
ylabel(y_label);
xlim([0,xlim1(2)]);
ylim([0,100]);

figure;
plot(g3(:,1),g3(:,2),...
	g3(:,1),g3(:,3),...
	g3(:,1),g3(:,4));
title('Bodyweight Squats');
xlabel(x_label);
ylabel(y_label);
xlim([0,xlim2(2)]);
ylim([0,100]);

figure;
plot(g4(:,1),g4(:,2),...
	g4(:,1),g4(:,3),...
	g4(:,1),g4(:,4));
title('Hip Abduction');
xlabel(x_label);
ylabel(y_label);
xlim([0,xlim1(2)]);
ylim([0,100]);






