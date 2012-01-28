function handlevel = f_handLevel(obj, frameNumber)
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here

v1= obj.groundPlane.dir;
v2= obj.getJointData(frameNumber, 'HAND_R') - obj.getJointData(frameNumber, 'HAND_L');

handlevel= acos(dot(v1, v2)./(norm(v1)*norm(v2)))*(180/pi);

end