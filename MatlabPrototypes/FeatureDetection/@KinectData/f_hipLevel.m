function hiplevel = f_hipLevel(obj, frameNumber)
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here

v1= obj.groundPlane.dir;
v2= obj.getJointData(frameNumber, 'HIP_R') - obj.getJointData(frameNumber, 'HIP_L');

hiplevel= acos(dot(v1, v2)./(norm(v1)*norm(v2)))*(180/pi);

end