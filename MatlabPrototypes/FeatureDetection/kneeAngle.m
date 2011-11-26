function [ angle ] = kneeAngle( hipRight, knee, foot)
    
    ba=knee-hipRight;
    bc=knee-foot;
    
    angle=acos(dot(ba./norm(ba),bc./norm(bc)))*180/pi;

end


