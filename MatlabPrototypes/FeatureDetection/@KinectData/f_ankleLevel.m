function anklelevel = f_ankleLevel(obj, frameNumber)
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here

v1= obj.groundPlane.dir;
v2= obj.getJointData(frameNumber, 'ANKLE_R') - obj.getJointData(frameNumber, 'ANKLE_L');

anklelevel= acos(dot(v1, v2)./(norm(v1)*norm(v2)))*(180/pi);

end