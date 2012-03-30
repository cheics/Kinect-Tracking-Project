%% initialize workspace function

[tf_squats,tc_squats]=getTrainingData('squats', 1);
[tf_armRaise,tc_armRaise]=getTrainingData('armRaise', 1);
[tf_legRaise,tc_legRaise]=getTrainingData('legRaise', 1);
[tf_legExt,tc_legExt]=getTrainingData('legExt', 1);

%% Retrain classifier?
cl_squats=MakeClassifier('BAYES', false, 'squats');
cl_armRaise=MakeClassifier('BAYES', false, 'armRaise');
cl_legRaise=MakeClassifier('BAYES', false, 'legRaise');
cl_legExt=MakeClassifier('BAYES', false, 'legExt');
cl_squats.trainClassifiers(tf_squats,tc_squats);
cl_armRaise.trainClassifiers(tf_armRaise,tc_armRaise);
cl_legRaise.trainClassifiers(tf_legRaise,tc_legRaise);
cl_legExt.trainClassifiers(tf_legExt,tc_legExt);
% cl_squats.saveClassifier()
% cl_armRaise.saveClassifier()
% cl_legRaise.saveClassifier()
% cl_legExt.saveClassifier()
% cl_squats=MakeClassifier('BAYES', true);
% cl_armRaise=MakeClassifier('BAYES', true);
% cl_legRaise=MakeClassifier('BAYES', true);
% cl_legExt=MakeClassifier('BAYES', true);

%% Import data for the KDE visualization
load SavedKDE_data/squat_KDE.mat
load SavedKDE_data/armRaise_KDE.mat
load SavedKDE_data/legRaise_KDE.mat
load SavedKDE_data/legExt_KDE.mat


