cc=FactoryKinectData('../../DataFiles/CritComp_squats1/squat000-0001.mat');
cc.GetFeatures();
%cc.DebugPose(1);
%cc.DebugPoseShowFeature(200, {'ELBOW_R', 'SHOULDER_R', 'SHOULDER_C'});
cc.DebugPoseShowFeature(400, {'ANKLE_R', 'KNEE_R', 'HIP_R'});
%cc.DebugPoseShowFeature(200, {'ELBOW_L', 'SHOULDER_L', 'SHOULDER_C'});

%cc.DebugAll(false);