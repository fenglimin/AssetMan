from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
import time

s = Service(ChromeDriverManager().install())
driver = webdriver.Chrome(service=s)
driver.get('http://www.citicbank.com/personal/investment/lcjdxcpxx/zycpcx/')
selectFundType = Select(driver.find_element_by_class_name('cms_proxy_cplb'))

# select by visible text
selectFundType.select_by_visible_text('理财')

inputCtrl = driver.find_element_by_class_name('cms_proxy_vague_srh_input')
actionCtrl = driver.find_element_by_class_name('cms_proxy_srh_btn')
resultFile = open('中信自营净值.txt', 'w')

with open('中信自营产品.txt', 'r') as fundFile:
    fundList = fundFile.readlines()
    for fund in fundList:
        inputCtrl.clear()
        inputCtrl.send_keys(fund)
        actionCtrl.click()
        time.sleep(3)
        result = driver.find_elements_by_xpath("//td")
        resultFile.write(result[9].text)
        resultFile.write(' ')
        resultFile.write(fund)        

resultFile.close()
