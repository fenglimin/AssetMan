from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
import time

s = Service(ChromeDriverManager().install())
driver = webdriver.Chrome(service=s)
resultFile = open('基金净值.txt', 'w')

with open('基金产品.txt', 'r') as fundFile:
    fundList = fundFile.readlines()
    for fund in fundList:
        driver.get('http://fund.10jqka.com.cn/' + fund + '/')
        time.sleep(3)
        netWorth = driver.find_element_by_class_name('percent')
        resultFile.write(netWorth.get_attribute('innerHTML'))
        resultFile.write(' ')
        resultFile.write(fund)
resultFile.close()
driver.close();
