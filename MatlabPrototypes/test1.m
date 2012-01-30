labels  = poseFeatures.classifierData;
data = poseFeatures.featureData;

stringlabel = zeros(length(labels),1);

names  = [000,001,002,010,011,012,020,021,022,100,101,110,111,102,112,120,121,122,200,201,202,...
          210,211,212,220,221,222];

for i = 1 : length(labels)
    
    stringlabel(i) = 100*labels(i,1) + 10*labels(i,2) + labels(i,3);
     
end

x = rand(1,15);
GEDscore = zeros(length(names),1);

for i = 1 : length(names)
    Gdata = data(find(stringlabel == (names(j))),:);
    u = mean(Gdata);
    covar = cov(Gdata);
    G = (x - u) * (inv(covar)) * transpose(x - u);
    GEDscore(i) = G^0.5;
end

